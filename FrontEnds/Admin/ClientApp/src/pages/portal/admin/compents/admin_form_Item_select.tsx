import React from 'react';
import { FormItemFactoryType, FormItemFactoryProps } from '@/components/form/form_item_factory';
import FormItemSelectElement from '@/components/form/form_item_select_element';

import { searchAdmins } from '../service';
import { AdminInfo } from '../data_d';

export default function AdminFormItemSelect({
  onChange,
  value,
}: {
  onChange?: (v: AdminInfo) => void;
  value?: AdminInfo;
}) {
  const searchFormItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '名称',
      name: 'admin_name',
    },
  ];
  const tableColumns = [
    {
      title: '名称',
      dataIndex: 'admin_name',
    },
  ];

  return (
    <FormItemSelectElement<AdminInfo>
      title="选择对应管理员信息"
      width={800}
      display={(a) => a?.admin_name}
      search_fetch={searchAdmins}
      table_columns={tableColumns}
      search_form_items={searchFormItems}
      onChange={onChange}
      value={value}
    ></FormItemSelectElement>
  );
}
