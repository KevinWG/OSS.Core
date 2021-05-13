import { parse } from 'querystring';
import moment from 'moment';
import { RespData } from './resp_d';
import { message } from 'antd';


const reg = /(((^https?:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+(?::\d+)?|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)$/;

export const isUrl = (path: string): boolean => reg.test(path);

export const getPageQuery = () => parse(window.location.href.split('?')[1]);

export const formatTimestamp = (time: number) => {
  return moment(time * 1000).format('YYYY-MM-DD');
};

export function CompareNotIn<S, T>(source: S[], target: T[], func: (s: S, t: T) => boolean) {
  const result: S[] = [];
  for (let index = 0; index < source.length; index++) {
    const s = source[index];

    let isHave = false;
    for (let tIndex = 0; tIndex < target.length; tIndex++) {
      const t = target[tIndex];

      if (func(s, t)) {
        isHave = true;
        break;
      }
    }
    if (!isHave) {
      result.push(s);
    }
  }
  return result;
}


export function getUploadPartialProps(getParaFunc:(fileName: string)=>Promise<RespData<GetUploadResp>>){
  return  {
    beforeUpload: function (file:any) {
        
      var promise = new Promise<void>((res, rej) => {
        getParaFunc(file.name).then((rData) => {
          if (rData.is_ok) {
            file.upload_paras=rData.data;
            res();
          } else {
            message.error(rData.msg);
            rej();
          }
        });
      });
      return promise;
    },
    data: function getUploadData(file:any) {
      return file.upload_paras.paras;
    },
    action:  function getUploadAction(file:any) {
      return file.upload_paras.upload_address;
    },
    onChange:  function onChange(info: any) {
      if(info.file.status=="done"){
        info.file.thumbUrl=info.file.upload_paras.access_url;
      }
    }
  }
}

export function getUrlsFromUploadFileList(fileList:any){
  const imgs:string[]=[];
  if (!fileList||fileList.length==0) {
    return imgs;
  }
  fileList.map((f:any)=>{
    let url:string=f.upload_paras.access_url;
    if (url.indexOf('?')>0) {
      url=url.substring(0,url.indexOf('?'));
    }
    if(f.status=="done"&&url){
      imgs.push(url);
    }
  });
  return imgs;
}

export function getUploadFilesByUrls(urls:string[],func:((index: number)=>string)=(i)=>('file'+i)){
  
  const files:any=[];
  if (!urls||urls.length==0) {
    return files;
  }
  urls.map((u:string,index:number)=>{   
    let url= u.indexOf('?')>0?u.substring(0,u.indexOf('?')):u;
    files.push({
      uid: index.toString(),
      name: func(index)||('file'+index),
      status: 'done',
      upload_paras:{
        access_url: url
      },
      url: url,
    });
  });
  return files;
}