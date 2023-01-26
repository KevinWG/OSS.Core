import { ProForm, ProFormText, ProFormTextArea } from "@ant-design/pro-components";
import { Form, message, Modal, ModalProps } from "antd";
import { useEffect } from "react";
import { add, update } from "../service";

interface EditWarehouseProps extends IEditorProps<WmsApi.Warehouse>, ModalProps {
  parent: WmsApi.Warehouse
}

export default function EditWarehouse({ record, callback, parent, ...restProps }: EditWarehouseProps) {
  const isAdd = record?.id && record.id == '0';
  const [editForm] = Form.useForm();

  const onFinish = async (req: any) => {
    if (parent.id !== "-1")
      req.parent_id = parent.id;

    var res = isAdd ? await add(req) : await update(record.id, req);
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

  return <Modal footer={false} {...restProps}>
    <ProForm form={editForm} onFinish={onFinish} initialValues={record}>
      <ProFormText width="md" name="name" label="名称" rules={[{ required: true,type:"string",max:30, message: "名称不能为空或超过30个字符" }]} />
      <ProFormTextArea rules={[{ type:"string",max:200, message: "备注不能超过200个字符" }]} width="md" name="remark" label="备注" />
    </ProForm>
  </Modal>
}