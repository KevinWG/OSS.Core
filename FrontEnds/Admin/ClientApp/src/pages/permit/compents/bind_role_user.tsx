import React from 'react';
import { DrawerProps } from 'antd/lib/drawer';

import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';
import EditDrawerForm from '@/components/form/eidt_drawer_form';
import RoleFormItemSelect from './role_form_Item_select';
import AdminFormItemSelect from '@/pages/portal/admin/compents/admin_form_Item_select';

import { Resp } from '@/utils/resp_d';
import { addRoleBind } from '../service';

interface BindRoleUserProps extends DrawerProps {
  callback: (res: Resp) => void;
}

const BindRoleUser = (props: BindRoleUserProps) => {
  const formItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.customer,
      label: '选择角色',
      name: 'role_info',
      custom_ele: <RoleFormItemSelect></RoleFormItemSelect>,
      rules: [{ required: true }],
    },
    {
      type: FormItemFactoryType.customer,
      label: '选择管理员',
      name: 'admin_info',
      custom_ele: <AdminFormItemSelect></AdminFormItemSelect>,
      rules: [{ required: true }],
    },
  ];

  const { callback, ...restProps } = props;
  return (
    <EditDrawerForm
      row_item_count={1}
      edit_fetch={addRoleBind}
      callback={callback}
      form_items={formItems}
      title={'添加用户角色绑定'}
      {...restProps}
    />
  );
};

export default BindRoleUser;
