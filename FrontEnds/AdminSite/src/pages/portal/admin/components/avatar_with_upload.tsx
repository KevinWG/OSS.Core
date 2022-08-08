import { getImgUploadProps } from '@/services/file/file_api';
import { updateNewAvatar } from '@/services/portal/admin_api';
import { UploadOutlined, UserOutlined } from '@ant-design/icons';
import { Avatar, Button, message, Space, Upload } from 'antd';
import React from 'react';
import { useModel } from 'umi';

const AvatarWithUpload: React.FC<{ is_mine: boolean }> = ({ is_mine }) => {
  const { initialState, setInitialState } = useModel('@@initialState');
  const currentUser = initialState?.currentUser || ({} as PortalApi.UserIdentity);

  function uploadChangeEvent(args: any) {
    var file = args.file;
    if (file.status == 'done') {
      const imgSrc = file.upload_paras.access_url;

      updateNewAvatar(imgSrc).then((res) => {
        if (res.success) {
          currentUser.avatar = imgSrc;
          setInitialState({ ...initialState, currentUser: currentUser });

          message.info('头像修改成功！');
        } else {
          message.info(res.msg);
        }
      });
    }
  }

  const uploadProps = getImgUploadProps();

  return (
    <Space direction="vertical">
      <Avatar size={120} src={currentUser.avatar} icon={<UserOutlined />} />
      <Upload method="POST" {...uploadProps} onChange={uploadChangeEvent}>
        {is_mine && (
          <Button>
            <UploadOutlined /> 上传/更换
          </Button>
        )}
      </Upload>
    </Space>
  );
};

export default AvatarWithUpload;
