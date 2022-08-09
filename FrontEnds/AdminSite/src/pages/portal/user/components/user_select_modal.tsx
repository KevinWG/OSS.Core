import SelectTableModalElement from '@/components/form/select_table_modal_element';
import { searchUsers } from '@/services/portal/user_api';
import { Avatar } from 'antd';

export default function UserSelectModal(props: {
  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  value?: any;

  /**
   * 自定义组件需要定义，由Form.Item 自动包装传值
   */
  onChange?: (v: any) => void;
}) {
  const cols = [
    {
      title: '昵称',
      dataIndex: 'nick_name',
    },
    {
      title: '邮箱',
      dataIndex: 'email',
    },
    {
      title: '手机号',
      dataIndex: 'mobile',
    },
  ];
  return (
    <SelectTableModalElement<PortalApi.UserInfo>
      display={(u) => {
        if (u)
          return (
            <div style={{ marginTop: 15, textAlign: 'center' }}>
              <Avatar size={100} src={u.avatar + '/s100'} />
              <div style={{ marginTop: 10 }}>{u.nick_name}</div>
            </div>
          );
        return undefined;
      }}
      params={{ status: '0' }}
      columns={cols}
      request={searchUsers}
      value_change_transform={(u) => u.id}
      {...props}
    ></SelectTableModalElement>
  );
}
