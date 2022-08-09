import { useRequest } from 'ahooks';
import { Button, Drawer, Form, message, Space } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import { useEffect } from 'react';

import EditForm from '@/components/form/edit_form';
import { FormItemFactoryProps } from '@/components/form/form_item_factory';

interface EditDrawerProps<T> extends DrawerProps {
  callback: (res: IResp) => void;

  edit_fetch: (newVals: any, oldRecord?: T) => Promise<IResp>;
  record?: T;
  form_items: FormItemFactoryProps[];
  row_item_count?: number;
}

export default function EditDrawerForm<T>(props: EditDrawerProps<T>) {
  const { callback, edit_fetch, record, form_items, visible, row_item_count, ...restProps } = props;

  const [editForm] = Form.useForm();
  const editReq = useRequest(edit_fetch, {
    manual: true,
    onSuccess: (res) => {
      if (res.success) {
        message.success('保存成功!');
        editForm.resetFields();
      }
      callback(res);
    },
  });

  function onFinish(vals: any) {
    editReq.run(vals, record);
  }

  // 因为 initialValues 的特殊，变化后这里需要重置一下
  useEffect(() => {
    if (visible) {
      editForm.resetFields();
    }
  }, [record]);

  return (
    <Drawer placement="right" width={680} visible={visible} {...restProps}>
      <EditForm
        name="DrawerEditForm"
        form={editForm}
        items={form_items}
        row_item_count={row_item_count}
        // initialValues={record}
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
              if (props.onClose) props.onClose({} as any);
            }}
          >
            返回
          </Button>
        </Space>
      </EditForm>
    </Drawer>
  );
}
