import TableFetchButtons from '@/components/button/table_Fetch_buttons';
import SearchProTable from '@/components/search/search_protable';
import FuncCodes from '@/services/common/func_codes';
import { AdminType } from '@/services/portal/enums';
import { addUserRoleBind, deleteUserRoleBind, getUserRoles } from '@/services/portal/role_api';
import { getUser } from '@/services/portal/user_api';
import { CloseCircleOutlined } from '@ant-design/icons';
import { ActionType, ProColumns, ProDescriptions } from '@ant-design/pro-components';
import { useAccess } from '@umijs/max';
import { useRequest } from 'ahooks';
import { Divider, Drawer, DrawerProps, message } from 'antd';
import { useEffect, useRef, useState } from 'react';
import RoleSelectModal from '../../permit/role/components/role_select_modal';

export default function AdminDetail({ admin_info, ...restProps }: AdminDetailProps) {
  const [userInfo, setUserInfo] = useState<PortalApi.UserInfo>();

  const userRoleBindTable = useRef<ActionType>();

  const statusButtons = [
    {
      condition: (r: PortalApi.RoleMo) => r.status == 0,
      buttons: [
        {
          btn_text: '取消绑定',
          fetch: (item: PortalApi.RoleMo) => deleteUserRoleBind(admin_info.id, item.id),
          fetch_desp: '取消用户角色绑定',
          // func_code: FuncCodes.Permit_RoleUserDelete,
          icon: <CloseCircleOutlined />,
        },
      ],
    },
  ];
  const roleBindCols: ProColumns<PortalApi.RoleMo>[] = [
    {
      title: '角色名称',
      dataIndex: 'name',
    },
    {
      title: '操作',
      dataIndex: 'id',
      hideInSearch: true,
      render: (_: any, r: PortalApi.RoleMo) => (
        <TableFetchButtons
          record={r}
          callback={(res, item, aName) => {
            if (res.success) userRoleBindTable.current?.reload();
          }}
          fetchKey={(item) => item.id}
          condition_buttons={statusButtons}
        ></TableFetchButtons>
      ),
    },
  ];

  useEffect(() => {
    if (admin_info.id && admin_info.id != '0') {
      getUser(admin_info.id).then((res) => {
        if (res.success) {
          setUserInfo(res.data);
        } else {
          setUserInfo(undefined);
        }
      });
    }
  }, [admin_info]);

  var bindUserRoleReq = useRequest(addUserRoleBind, {
    manual: true,
    onSuccess: (res) => {
      if (res.success) {
        message.success('绑定成功!');
        userRoleBindTable.current?.reload(true);
      }
    },
  });

  const access = useAccess();

  return (
    <Drawer width={680} title="管理员信息" {...restProps}>
      <Divider orientation="left" orientationMargin="0">
        基础信息
      </Divider>

      <ProDescriptions dataSource={admin_info}>
        <ProDescriptions.Item dataIndex={['admin_name']} label="名称" />
        <ProDescriptions.Item
          label="类型"
          dataIndex={['admin_type']}
          valueType="select"
          valueEnum={AdminType}
        />
      </ProDescriptions>

      <Divider orientation="left" orientationMargin="0">
        会员信息
      </Divider>

      <ProDescriptions dataSource={userInfo}>
        <ProDescriptions.Item dataIndex={['nick_name']} label="昵称" />
        <ProDescriptions.Item dataIndex={['email']} label="邮箱" />
        <ProDescriptions.Item dataIndex={['mobile']} label="手机号" />
      </ProDescriptions>

      {admin_info.admin_type != 100 && access[FuncCodes.portal_user_roles] && (
        <>
          <Divider orientation="left" orientationMargin="0">
            角色信息
          </Divider>
          <SearchProTable
            request={() => getUserRoles(admin_info.id)}
            search={false}
            pagination={false}
            rowKey="id"
            actionRef={userRoleBindTable}
            headerTitle={
              <RoleSelectModal
                loading={bindUserRoleReq.loading}
                onChange={(rId) => bindUserRoleReq.run(rId, admin_info.id)}
              ></RoleSelectModal>
            }
            columns={roleBindCols}
          ></SearchProTable>
        </>
      )}
    </Drawer>
  );
}

interface AdminDetailProps extends DrawerProps {
  admin_info: PortalApi.AdminInfo;
}
