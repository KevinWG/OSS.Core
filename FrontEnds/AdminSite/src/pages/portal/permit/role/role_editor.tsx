import { addRole, updateRoleName } from '@/services/permit/role_api';
import { ProForm, ProFormText } from '@ant-design/pro-components';
import { Form, message, Modal, ModalProps } from 'antd';

interface RoleEditorProps extends IEditorProps<PortalApi.RoleMo>, ModalProps {}

export default function RoleEditor({ record, call_back, ...restProps }: RoleEditorProps) {
  const isAdd = record?.id && record.id == '0';
  const [editForm] = Form.useForm();

  const submitHandler = async (req: any) => {
    var res = isAdd ? await addRole(req) : await updateRoleName(record.id, req);
    if (res.success) {
      message.success((isAdd ? '新增' : '修改') + '成功!');
      editForm.resetFields();
    }
    if (call_back) call_back(res, record);
  };

  // 因为 initialValues 的特殊，变化后这里需要重置一下
  //   useEffect(() => {
  //     editForm.resetFields();
  //   }, [record]);

  return (
    <Modal {...restProps} footer={false}>
      <ProForm onFinish={submitHandler} initialValues={record} form={editForm}>
        <ProFormText width="md" name="name" label="名称" />
      </ProForm>
    </Modal>
  );
}
