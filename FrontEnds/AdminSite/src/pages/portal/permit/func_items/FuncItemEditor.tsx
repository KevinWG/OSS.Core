import { addFuncItem, changeFuncItem } from '@/services/permit/func_api';
import { ProDescriptions, ProForm, ProFormText } from '@ant-design/pro-components';
import { Form, message, Modal, ModalProps } from 'antd';
import { useEffect } from 'react';

interface FuncItemEditorProps extends IEditorProps<PermitApi.FuncItem>, ModalProps {}

export default function FuncItemEditor({
  record,
  visible,
  callback,
  ...modelProps
}: FuncItemEditorProps) {
  const isAdd = record?.id == '0';
  const [editForm] = Form.useForm();

  const submitHandler = async (req: PermitApi.AddFuncItemReq) => {
    if (isAdd) {
      req.parent_code = record.code;
    }
    var res = isAdd ? await addFuncItem(req) : await changeFuncItem(record.code, req);
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
    <Modal title="新增/编辑 权限信息" visible={visible} footer={<></>} {...modelProps}>
      {isAdd && record.code && (
        <ProDescriptions size="small">
          <ProDescriptions.Item label="父级权限" valueType="text">
            {record.title + '(' + record.code + ')'}
          </ProDescriptions.Item>
        </ProDescriptions>
      )}

      <ProForm onFinish={submitHandler} initialValues={record} form={editForm}>
        {isAdd && <ProFormText width="md" name="code" label="编码" />}
        <ProFormText width="md" name="title" label="名称" />
      </ProForm>
    </Modal>
  );
}
