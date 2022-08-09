declare namespace FileApi {
  interface GetUploadResp {
    upload_address: string;
    paras: { [key: string]: string };
    access_url: string;
  }
}
