interface GetUploadResp{
    upload_url:string,
    paras:UploadPara,    
  }

  interface UploadPara{
    key:string,
    OSSAccessKeyId:string,
    policy:string,
    Signature:string
  }