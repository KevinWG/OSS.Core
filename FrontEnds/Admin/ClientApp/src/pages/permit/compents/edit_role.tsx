import React from 'react';
import { DrawerProps } from 'antd/lib/drawer';

import { Resp } from '@/utils/resp_d';

import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';

import { RoleInfo } from '../data_d';
import { roleEdit } from '../service';
import EditDrawerForm from '@/components/form/eidt_drawer_form';

interface EditRoleProps extends DrawerProps {
  callback: (res: Resp, isAdd: boolean) => void;
  record?: RoleInfo;
}

const formItems: FormItemFactoryProps[] = [
  {
    type: FormItemFactoryType.input,
    label: '角色名称',
    name: 'name',
    rules: [
      { type: 'string', required: true, max: 12, message: '角色名称必填（长度不超过12的字）' },
      { pattern: /^[\u4e00-\u9fa5_a-zA-Z0-9]+$/, message: '角色名称仅限于中英文数字' },
    ],
  },
];

const EditRole = (props: EditRoleProps) => {
  const { callback, record, ...restProps } = props;
  const opName = record ? '修改' : '创建';

  return (
    <EditDrawerForm
      edit_fetch={roleEdit}
      record={record}
      callback={(res) => callback(res, !record)}
      form_items={formItems}
      title={opName + '角色'}
      {...restProps}
    />
  );
};

export default EditRole;
