import AccessButton from '@/components/button/access_button';
import SelectTableModalElement from '@/components/form/select_table_modal_element';
import FuncCodes from '@/services/common/func_codes';
import { searchRoles } from '@/services/portal/role_api';

export default function RoleSelectModal(props: {
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  value?: any;

  loading?: boolean;

  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  onChange?: (v: any) => void;
}) {
  const cols = [
    {
      title: '角色名称',
      dataIndex: 'name',
    },
  ];
  return (
    <SelectTableModalElement<PortalApi.RoleMo>
      display={(r) => (
        <AccessButton
          func_code={FuncCodes.portal_role_bind_user}
          loading={props.loading}
          type="primary"
        >
          绑定角色
        </AccessButton>
      )}
      params={{ status: '0' }}
      columns={cols}
      request={searchRoles}
      value_change_transform={(u) => u.id}
      {...props}
    ></SelectTableModalElement>
  );
}
