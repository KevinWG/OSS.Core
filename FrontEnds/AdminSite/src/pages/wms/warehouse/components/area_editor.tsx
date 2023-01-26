import { ProForm, ProFormText, ProFormTextArea } from '@ant-design/pro-components';
import { Form, message } from 'antd';
import Modal, { ModalProps } from 'antd/lib/modal/Modal';
import { useEffect } from 'react';
import { addArea, checkAreaCode, updateArea } from '../service';

interface AreaEditorProps extends IEditorProps<WmsApi.WareArea>, ModalProps {
  warehouse: WmsApi.Warehouse;
}

export default function AreaEditor({ record, warehouse, callback, ...restProps }: AreaEditorProps) {
  const isAdd = record?.id && record.id == '0';
  const [editForm] = Form.useForm();

  const onFinish = async (req: any) => {
    if (isAdd) req.warehouse_id = warehouse.id;

    var res = isAdd ? await addArea(req) : await updateArea(record.id, req);

    if (res.success) {
      message.success((isAdd ? '新增' : '修改') + '成功!');
      editForm.resetFields();
    }
    if (callback) callback(res, record);
  };

  // 因为 initialValues 的特殊，变化后这里需要重置一下
  useEffect(() => {
    editForm.resetFields();
  }, [record]);

  return (
    <Modal {...restProps} footer={false}>
      <ProForm form={editForm} onFinish={onFinish} initialValues={record}>
        {isAdd && (
          <ProFormText
            width="md"
            name="code"
            label="编码"
            rules={[
              {
                required: true,
                type: 'string',
                max: 30,
                message: '编码不能为空且不能超过30个字符',
              },
              () => ({
                validator: (r: any, val: any) => checkAreaCode(warehouse.id, val),
              }),
            ]}
          />
        )}
        <ProFormTextArea
          rules={[{ type: 'string', max: 200, message: '备注不能超过200个字符' }]}
          width="md"
          name="remark"
          label="备注"
        />
      </ProForm>
    </Modal>
  );
}
