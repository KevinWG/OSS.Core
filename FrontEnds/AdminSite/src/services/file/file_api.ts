import { request } from '@umijs/max';
import { message } from 'antd';
import { UploadChangeParam } from 'antd/lib/upload';

/**
 * 获取通用的图片上传的控件传递参数
 * @returns
 */
export function getImgUploadProps(changeEvent?: (info: UploadChangeParam) => void) {
  return getUploadPropsByParaFunc((filename: string) => {
    return request<IRespData<FileApi.GetUploadResp>>(
      '/api/p/file/GetImgUploadParas?name=' + filename,
    );
  }, changeEvent);
}

/**
 * 获取上传控件的传递参数
 * @param getParaFunc 获取服务器上传参数
 * @returns
 */
export function getUploadPropsByParaFunc(
  getParaFunc: (fileName: string) => Promise<IRespData<FileApi.GetUploadResp>>,
  changeEvent?: (info: UploadChangeParam) => void,
) {
  return {
    beforeUpload: function (file: any) {
      var promise = new Promise<void>((res, rej) => {
        getParaFunc(file.name).then((rData) => {
          if (rData.success) {
            file.upload_paras = rData.data;
            res();
          } else {
            message.error(rData.msg);
            rej();
          }
        });
      });
      return promise;
    },
    data: function getUploadData(file: any) {
      return file.upload_paras.paras;
    },
    action: function getUploadAction(file: any) {
      return file.upload_paras.upload_address;
    },
    onChange: function onChange(info: any) {
      if (info.file.status == 'done') {
        info.file.thumbUrl = info.file.upload_paras.access_url;
      }
      if (changeEvent) {
        changeEvent(info);
      }
    },
  };
}
