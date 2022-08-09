import AccessButton from '@/components/button/access_button';
import FuncCodes from '@/services/common/func_codes';
import { CompareNotIn } from '@/services/common/utils';
import { getAllFuncItems } from '@/services/permit/func_api';
import { editRoleFuncs, getRoleFuncs } from '@/services/portal/role_api';
import { useRequest } from 'ahooks';
import { Button, Card, Col, Drawer, message, Row, Space, Tree } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import { DataNode } from 'antd/lib/tree';
import React, { useEffect, useRef, useState } from 'react';

function getTreeNodes(funcItems: PermitApi.FuncItem[], parentCode?: string): DataNode[] {
  const trees: DataNode[] = [];

  for (let index = 0; index < funcItems.length; index++) {
    const ele = funcItems[index];

    if (ele.parent_code == parentCode) {
      const treeItem: DataNode = { key: ele.code, title: ele.title };
      treeItem.children = getTreeNodes(funcItems, ele.code);

      trees.push(treeItem);
    }
  }
  return trees;
}

function getRoleTreeNodes(
  treeNodes?: DataNode[],
  roleFuncItems?: PortalApi.RoleFuncItem[],
  needEdit?: boolean,
) {
  if (!treeNodes) return [];

  if (!roleFuncItems) {
    roleFuncItems = [];
  }
  const roleTreeNodes: DataNode[] = [];

  for (let index = 0; index < treeNodes.length; index++) {
    const treeItem = treeNodes[index];

    let isOwner = false;
    const roleTreeItem: DataNode = { key: treeItem.key, title: treeItem.title };

    for (let rIndex = 0; rIndex < roleFuncItems.length; rIndex++) {
      const roleFuncItem = roleFuncItems[rIndex];

      const children = getRoleTreeNodes(treeItem.children, roleFuncItems, needEdit);
      if ((children && children?.length > 0) || treeItem.key == roleFuncItem.func_code) {
        isOwner = true;
        roleTreeItem.children = children;
        roleTreeNodes.push(roleTreeItem);
        break;
      }
    }
    if (needEdit && !isOwner) {
      roleTreeItem.children = getRoleTreeNodes(treeItem.children, roleFuncItems, needEdit);
      roleTreeNodes.push(roleTreeItem);
    }
  }
  return roleTreeNodes;
}

// todo  获取权限列表
interface EditRoleFuncProps extends DrawerProps {
  callback: (res?: IResp) => void;
  record?: PortalApi.RoleMo;
  show_type: 'show_role' | 'show_all';
}

const EditRoleFunc: React.FC<EditRoleFuncProps> = (props: EditRoleFuncProps) => {
  const { callback, record, visible, show_type, ...restProps } = props;

  const [needEdit, setNeedEdit] = useState(false); //  是否编辑状态
  const [treeNodes, setTreeNodes] = useState<DataNode[]>(); //  树形节点数据
  const [checkedKeys, setCheckedKeys] = useState<string[]>(); // 当前已经选择的数据

  const sysFuncTreeDataRef = useRef<DataNode[]>();
  const roleFuncItemsRef = useRef<PortalApi.RoleFuncItem[]>();

  const sysFuncReq = useRequest(getAllFuncItems, {
    manual: true,
    onSuccess: (listRes) => {
      if (listRes.success) {
        sysFuncTreeDataRef.current = listRes.success ? getTreeNodes(listRes.data ?? []) : [];
      }
    },
  });

  const roleFuncReq = useRequest(getRoleFuncs, {
    manual: true,
    onSuccess: (rfListRes) => {
      roleFuncItemsRef.current = rfListRes.data;
      const roleTreeData = getRoleTreeNodes(sysFuncTreeDataRef.current, roleFuncItemsRef.current);
      setTreeNodes(roleTreeData);
    },
  });

  const editFuncReq = useRequest(editRoleFuncs, {
    manual: true,
    onSuccess: (res) => {
      if (res.success) {
        message.info('保存成功！');
        setNeedEdit(false);
        showRoleFuncItems(record?.id);
      }
    },
  });

  // 初始化系统权限信息,页面初始化的时候就执行
  useEffect(() => {
    sysFuncReq.run();
  }, []);

  // 加载对应的角色权限列表信息
  useEffect(() => {
    setNeedEdit(false);
    if (sysFuncTreeDataRef.current) {
      if (show_type == 'show_role' && record) {
        showRoleFuncItems(record.id);
      } else if (show_type == 'show_all') {
        setTreeNodes(sysFuncTreeDataRef.current);
      }
    }
  }, [record]);

  /**
   *
   * @param roleId 加载展示服务端角色对应权限信息
   */
  function showRoleFuncItems(roleId: string | undefined) {
    if (roleId) roleFuncReq.run(roleId);
  }

  const editClick = () => {
    const cKeys = roleFuncItemsRef.current?.map((rfItem) => rfItem.func_code);

    setCheckedKeys(cKeys);
    setNeedEdit(true);

    const roleTreeData = getRoleTreeNodes(
      sysFuncTreeDataRef.current,
      roleFuncItemsRef.current,
      true,
    );

    setTreeNodes(roleTreeData);
  };

  const cacelEditClick = () => {
    const roleTreeData = getRoleTreeNodes(
      sysFuncTreeDataRef.current,
      roleFuncItemsRef.current,
      false,
    );

    setNeedEdit(false);
    setTreeNodes(roleTreeData);
  };

  /**
   * 保存角色对应权限
   *  保存成功后，修改回正常的展示
   */
  function editSave() {
    const oldKeys = roleFuncItemsRef.current?.map((rfItem) => rfItem.func_code) ?? [];

    const newItems = CompareNotIn(checkedKeys ?? [], oldKeys, (s, t) => s == t);
    const deleteItems = CompareNotIn(oldKeys, checkedKeys ?? [], (s, t) => s == t);

    if (newItems.length == 0 && deleteItems.length == 0) {
      message.warn('当前选项无变化！');
      return;
    }

    if (record?.id) {
      editFuncReq.run(record.id, newItems, deleteItems);
    }
  }

  return (
    <Drawer
      placement="right"
      title={(record?.name ? record.name : '系统') + '权限详情'}
      width={640}
      visible={visible}
      {...restProps}
    >
      <Card loading={roleFuncReq.loading || sysFuncReq.loading}>
        {(!treeNodes?.length || treeNodes?.length == 0) && (
          <p style={{ textAlign: 'center' }}>
            {(show_type == 'show_role' ? '当前角色' : '系统') +
              '暂无权限配置，请点击下方编辑按钮配置！'}
          </p>
        )}

        <Tree
          treeData={treeNodes}
          defaultExpandAll={true}
          showLine={true}
          selectable={false}
          checkable={needEdit}
          onCheck={(k) => {
            console.info(k);
            setCheckedKeys(k.checked);
          }}
          checkStrictly={true}
          checkedKeys={checkedKeys}
        />
      </Card>

      <Row style={{ marginTop: 20 }}>
        <Col span={24} style={{ textAlign: 'right' }}>
          <Space>
            {show_type == 'show_role' && !needEdit && (
              <>
                <AccessButton
                  func_code={FuncCodes.portal_grant_role_change}
                  type="primary"
                  onClick={editClick}
                >
                  编辑
                </AccessButton>
                <Button type="default" onClick={() => callback()}>
                  返回
                </Button>
              </>
            )}
            {show_type == 'show_role' && needEdit && (
              <>
                <AccessButton
                  type="primary"
                  func_code={FuncCodes.portal_grant_role_change}
                  onClick={editSave}
                  loading={editFuncReq.loading}
                >
                  保存
                </AccessButton>
                <Button type="default" onClick={cacelEditClick}>
                  取消编辑
                </Button>
              </>
            )}
          </Space>
        </Col>
      </Row>
    </Drawer>
  );
};

export default EditRoleFunc;
