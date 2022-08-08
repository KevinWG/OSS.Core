import {
  addTemplate,
  NotifyChannel,
  NotifyType,
  updateTemplate,
} from '@/services/notify/template_api';
import {
  ProForm,
  ProFormRadio,
  ProFormSelect,
  ProFormText,
  ProFormTextArea,
} from '@ant-design/pro-components';
import { Form, message, Modal, ModalProps, Row, Space } from 'antd';
import { useEffect } from 'react';
interface TemplateEditorProps extends IEditorProps<NotifyApi.Template>, ModalProps {}

export default function ({ record, visible, call_back, ...modelProps }: TemplateEditorProps) {
  const [editForm] = Form.useForm();

  // 因为 initialValues 的特殊，变化后这里需要重置一下
  useEffect(() => {
    if (visible) {
      editForm.resetFields();
    }
  }, [record]);

  const isAdd = record.id == '0';
  const submit = async (req: NotifyApi.AddTemplateReq) => {
    var res = isAdd ? await addTemplate(req) : await updateTemplate(record.id, req);
    if (res.success) {
      message.success((isAdd ? '新增' : '修改') + '成功!');
      editForm.resetFields();
    }
    if (call_back) call_back(res, record);
  };

  return (
    <Modal footer={null} width={680} visible={visible} {...modelProps}>
      <ProForm
        submitter={{
          render: (_, dom) => {
            return (
              <Row justify="space-around">
                <Space>{dom}</Space>
              </Row>
            );
          },
        }}
        initialValues={record}
        onFinish={submit}
        form={editForm}
        grid={true}
        rowProps={{ gutter: 30 }}
      >
        <ProFormText
          name="title"
          label="标题"
          rules={[
            { required: true, message: '请填写标题' },
            { max: 100, message: '标题不能超过100个字符' },
          ]}
        />
        <ProFormSelect
          rules={[{ required: true, message: '请选择发送类型' }]}
          colProps={{ xs: 12, md: 12 }}
          options={NotifyType}
          name="notify_type"
          label="发送类型"
        />
        <ProFormSelect
          rules={[{ required: true, message: '请选择推送通道' }]}
          colProps={{ xs: 12, md: 12 }}
          options={NotifyChannel}
          name="notify_channel"
          label="推送通道"
        />
        <ProFormText
          colProps={{ xs: 12, md: 12 }}
          name="channel_template_code"
          label="通道模板编号"
          tooltip="对接的推送通道提供的模板编号，如阿里云，华为等平台内申请的模板编号"
        />
        <ProFormText colProps={{ xs: 12, md: 12 }} name="channel_sender" label="通道发送账号" />
        <ProFormText colProps={{ xs: 12, md: 12 }} name="sign_name" label="发送签名" />
        <ProFormTextArea
          name="content"
          label="模板内容"
          tooltip={
            <>
              "如果已在第三方通道设置并填写模板编号，此项可为空。
              <br />
              如果涉及业务变量，请和业务调用方做好沟通，以“ {'{变量名}'} ”形式在内容中完成设置。"
            </>
          }
        />
        <ProFormRadio.Group
          colProps={{ xs: 12, md: 12 }}
          name="is_html"
          label="是否是Html"
          tooltip="内容格式是否是Html，发送邮件时"
          radioType="button"
          options={[
            { label: '是', value: 1 },
            { label: '否', value: 0 },
          ]}
        />
      </ProForm>
    </Modal>
  );
}
