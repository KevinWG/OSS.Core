import React from 'react';
import { FormItemFactoryType, FormItemFactoryProps } from '@/components/form/form_item_factory';
import FormItemSelectElement from '@/components/form/form_item_select_element';

import { searchRoles } from '../service';
import { RoleInfo } from '../data_d';

interface RoleSelectModalProps {
  value?: RoleInfo;
  onChange?: (v: RoleInfo) => void;
}

export default function RoleFormItemSelect({ onChange, value }: RoleSelectModalProps) {
  const searchFormItems: FormItemFactoryProps[] = [
    {
      type: FormItemFactoryType.input,
      label: '名称',
      name: 'name',
    },
  ];

  const tableColumns = [
    {
      title: '名称',
      dataIndex: 'name',
    },
  ];

  return (
    <FormItemSelectElement<RoleInfo>
      title={'选择对应角色信息'}
      width={800}
      display={(a) => a?.name}
      search_fetch={searchRoles}
      table_columns={tableColumns}
      search_form_items={searchFormItems}
      onChange={onChange}
      value={value}
    ></FormItemSelectElement>
  );
}
