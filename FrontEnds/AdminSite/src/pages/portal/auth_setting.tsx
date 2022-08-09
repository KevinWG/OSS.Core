import { getAuthSetting, saveAuthSetting } from '@/services/portal/setting_api';
import {
  PageContainer,
  ProDescriptions,
  ProDescriptionsActionType,
} from '@ant-design/pro-components';
import { useRef } from 'react';

export default function () {
  const actionRef = useRef<ProDescriptionsActionType>();
  return (
    <PageContainer>
      <ProDescriptions
        actionRef={actionRef}
        column={{ xs: 1, md: 2 }}
        request={getAuthSetting}
        editable={{
          onSave: (keypath, newInfo, oriInfo) => saveAuthSetting(newInfo as any),
        }}
      >
        <ProDescriptions.Item
          dataIndex={['SmsTemplateId']}
          label="注册/登录 短信模板Id"
          tooltip="如果模板选择系统测试通道,在接口返回消息或浏览器Console中查看code信息"
        />
        <ProDescriptions.Item
          dataIndex={['EmailTemplateId']}
          label="注册/登录 邮件模板Id"
          tooltip="如果模板选择系统测试通道,在接口返回消息或浏览器Console中查看code信息"
        />
      </ProDescriptions>
    </PageContainer>
  );
}
