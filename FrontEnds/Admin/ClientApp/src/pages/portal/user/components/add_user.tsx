import React from 'react';
import { DrawerProps } from 'antd/lib/drawer';
import { FormItemFactoryProps, FormItemFactoryType } from '@/components/form/form_item_factory';
import { Resp } from '@/utils/resp_d';
import EditDrawerForm from '@/components/form/eidt_drawer_form';
import { addUser, checkRegName } from '../service';

interface AddUserProps extends DrawerProps {
  callback: (res: Resp) => void;
}
async function CheckValid(type: number, val: any) {
  if (!val) {
    return Promise.resolve();
  }
  if (type == 20) {
    const isMobile = /^1[3-9]{1}\d{9}$/.test(val);
    if (!isMobile) {
      return Promise.reject('请输入正确手机号！');
    }
  }
  if (type == 40) {
    const isEmail = /^\w+([-.]?\w*)@\w+([-.]?\w*).\w+$/.test(val);
    if (!isEmail) {
      return Promise.reject('请输入正确邮箱！');
    }
  }
  var res = await checkRegName({ reg_type: type, reg_name: val });
  if (res.is_ok) {
    return Promise.resolve();
  } else {
    return Promise.reject(res.msg);
  }
}

const formItems: FormItemFactoryProps[] = [
  {
    type: FormItemFactoryType.input,
    label: '用户名称',
    name: 'nick_name',
    col_span: 24,
    rules: [
      { type: 'string', required: true, max: 20, message: '用户名称必填切不超过20个字符' },
      { pattern: /^[\u4e00-\u9fa5_a-zA-Z0-9]+$/, message: '角色名称仅限于中英文数字' },
    ],
  },
  {
    type: FormItemFactoryType.input,
    label: '手机号',
    name: 'mobile',
    col_span: 24,
    rules: [{ validator: (r: any, val: any) => CheckValid(20, val) }],
  },
  {
    type: FormItemFactoryType.input,
    label: '邮箱',
    name: 'email',
    col_span: 24,
    rules: [{ validator: (r: any, val: any) => CheckValid(40, val) }],
  },
];

const AddUser = (props: AddUserProps) => {
  const { callback, ...restProps } = props;
  return (
    <EditDrawerForm
      edit_fetch={addUser}
      callback={(res) => callback(res)}
      form_items={formItems}
      title="创建用户"
      {...restProps}
    />
  );
};

export default AddUser;
