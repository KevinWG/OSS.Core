import React, { useEffect, useState, useRef } from 'react';
import { Drawer, Tree, Row, Card, Button, Col, message, Space } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import { RoleInfo, FuncItem, RoleFuncItem } from '../data_d';
import { Resp, FuncCodes } from '@/utils/resp_d';
import { useRequest } from 'umi';
import { getSysAllFuncs, getRoleFuncs, editRoleFuncs } from '../service';
import { DataNode } from 'antd/lib/tree';
import { CompareNotIn } from '@/utils/utils';
import AccessButton from '@/components/Button/access_button';

function getTreeNodes(funcItems: FuncItem[], parentCode?: string): DataNode[] {
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
  roleFuncItems?: RoleFuncItem[],
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
  callback: (res?: Resp) => void;
  record?: RoleInfo;
  show_type: 'show_role' | 'show_all';
}

const EditRoleFunc: React.FC<EditRoleFuncProps> = (props: EditRoleFuncProps) => {
  const { callback, record, visible, show_type, ...restProps } = props;

  const [needEdit, setNeedEdit] = useState(false); //  是否编辑状态
  const [treeNodes, setTreeNodes] = useState<DataNode[]>(); //  树形节点数据
  const [checkedKeys, setCheckedKeys] = useState<string[]>(); // 当前已经选择的数据

  const sysFuncTreeDataRef = useRef<DataNode[]>();
  const roleFuncItemsRef = useRef<RoleFuncItem[]>();

  const sysFuncReq = useRequest(getSysAllFuncs, { manual: true });
  const roleFuncReq = useRequest(getRoleFuncs, { manual: true });
  const editFuncReq = useRequest(editRoleFuncs, { manual: true });

  // 初始化系统权限信息,页面初始化的时候就执行
  useEffect(() => {
    sysFuncReq.run().then((listRes) => {
      if (listRes.is_ok) {
        sysFuncTreeDataRef.current = listRes.is_ok ? getTreeNodes(listRes.data ?? []) : [];
        setTreeNodes(sysFuncTreeDataRef.current);
      }
    });
  }, []);

  // 加载对应的角色权限列表信息
  useEffect(() => {
    setNeedEdit(false);
    // setTreeNodes([]);
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
  function showRoleFuncItems(roleId: string) {
    roleFuncReq.run(roleId).then((rfListRes) => {
      roleFuncItemsRef.current = rfListRes.data;
      const roleTreeData = getRoleTreeNodes(sysFuncTreeDataRef.current, roleFuncItemsRef.current);
      setTreeNodes(roleTreeData);
    });
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
    editFuncReq.run(record.id, newItems, deleteItems).then((res) => {
      if (res.is_ok) {
        message.info('保存成功！');

        setNeedEdit(false);
        showRoleFuncItems(record.id);
      }
    });
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
            {' '}
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
                  func_code={FuncCodes.Permit_RoleFuncChange}
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
                  func_code={FuncCodes.Permit_RoleFuncChange}
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
