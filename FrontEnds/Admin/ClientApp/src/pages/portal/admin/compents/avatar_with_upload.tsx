import React from 'react';
import { Avatar, message, Space, Upload, Button } from 'antd';
import { getAvatarUploadPara, updateNewAvatar } from '../service';
import { useState } from 'react';
import { UserOutlined, UploadOutlined } from '@ant-design/icons';
import { UploadProps } from 'antd/lib/upload';
import { AdminIdentity } from '../../login/service';
import { useModel } from 'umi';

const AvatarWithUpload: React.FC<{ is_mine: boolean }> = ({ is_mine }) => {
  const [uploadPara, setUploadPara] = useState({} as GetUploadResp);
  const [uploading, setUploading] = useState(false);
  const { initialState, setInitialState } = useModel('@@initialState');
  const currentUser = initialState?.currentUser || ({} as AdminIdentity);

  const beforUpload = function (file:any) {
    setUploading(true);

    var promise = new Promise<void>((res, rej) => {
      getAvatarUploadPara(file.name).then((rData) => {
        if (rData.is_ok) {
          setUploadPara(rData.data);
          res();
        } else {
          message.error(rData.msg);
          rej();
        }
      });
    });
    return promise;
  };

  function getUploadAction() {
    return uploadPara.upload_address;
  }
  function getUploadData() {
    return uploadPara.paras;
  }
  function onChange(info: any) {
    if (info.file.status == 'done') {
      
      var imgSrc = uploadPara.access_url;
      updateNewAvatar(imgSrc)
        .then((res) => {
          if (res.is_ok) {
            currentUser.avatar = imgSrc;

            setInitialState({ ...initialState, currentUser: currentUser });

            message.info('头像修改成功！');
          } else {
            message.info(res.msg);
          }
        })
        .finally(() => {
          setUploading(false);
        });
    } else if (info.file.status == 'error') {
      message.info('上传失败！');
      setUploading(false);
    } 
  }

  const uploadProps: UploadProps = {
    showUploadList: false,
    beforeUpload: beforUpload,
    data:getUploadData,
    action: getUploadAction,
    onChange: onChange,
    // headers:{"Content-Type":""}
  };
  return (
    <Space direction="vertical">
      <Avatar size={120} src={currentUser.avatar } icon={<UserOutlined />} />
      <Upload method="POST" {...uploadProps}>
        {is_mine && (
          <Button loading={uploading}>
            <UploadOutlined /> 上传/更换
          </Button>
        )}
      </Upload>
    </Space>
  );
};

export default AvatarWithUpload;
