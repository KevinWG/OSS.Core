import React, { useEffect } from 'react';
import { Drawer, Button, Space, Form, message } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import { useRequest } from 'umi';

import { Resp } from '@/utils/resp_d';

import EditForm from '@/components/form/edit_form';
import { FormItemFactoryProps } from '@/components/form/form_item_factory';


interface EditDrawerProps<T> extends DrawerProps {
  callback: (res: Resp) => void;
  
  edit_fetch: (newVals: any, oldRecord?: T) => Promise<Resp>;
  record?: T;
  form_items: FormItemFactoryProps[];
  row_item_count?: number;
}

export default function EditDrawerForm<T>(props: EditDrawerProps<T>) {
  const { callback, edit_fetch, record, form_items, visible, row_item_count, ...restProps } = props;

  const [editForm] = Form.useForm();
  const editReq = useRequest(edit_fetch, {
    manual: true,
  });

  async function onFinish(vals: any) {
    const res = await editReq.run(vals, record);
    if (res.is_failed) {
      message.error(res.msg);
    } else {
      message.success('保存成功!');
      editForm.resetFields();
      props.onClose && props.onClose();
    }
    callback(res);
  }

  // 因为 initialValues 的特殊，变化后这里需要重置一下
  useEffect(() => {
    if(visible){
      editForm.resetFields();
    }
  }, [record]);

  return (
    <Drawer placement="right" width={680} visible={visible} {...restProps}>
      <EditForm name="DrawerEditForm"
        form={editForm}
        items={form_items}
        row_item_count={row_item_count}
        initialValues={record}
        onFinish={onFinish}
      >
        <Space>
          <Button type="primary" htmlType="submit" loading={editReq.loading}>
            保存
          </Button>
          <Button
            type="default"
            onClick={() => {
              editForm.resetFields();
              if (props.onClose) props.onClose();
            }}
          >
            返回
          </Button>
        </Space>
      </EditForm>
    </Drawer>
  );
}
