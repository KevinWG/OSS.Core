
import { Upload } from "antd";
import { request } from "umi";
import React, { useState } from "react";

import 'braft-editor/dist/index.css';
import BraftEditor, { BraftEditorProps } from "braft-editor";
import { ContentUtils } from 'braft-utils'

import { RespData } from "@/utils/resp_d";
import { getUploadPartialProps } from "@/utils/utils";

export interface RichEditorProps extends BraftEditorProps {

}


const uploadInfoPara = getUploadPartialProps(getRichUploadPara);

export default function RichEditorItem(props: RichEditorProps) {

    const { value, onChange, ...restProps } = props;
    const [editorStatus, setEditorStatus] = useState(value ? value : BraftEditor.createEditorState(null));
    
    function interUpdate(newValue: any) {
        setEditorStatus(newValue)
        if (onChange) {
            onChange(newValue)
        }   
    }

    function onUploadChange(info: any) {
        if (info.file.status == "done") {
            const newValue = ContentUtils.insertMedias(editorStatus, [{
                type: 'IMAGE',
                url: info.file.upload_paras.access_url
            }]);
            interUpdate(newValue);
        }
    }
    const extendControls: any = [
        {
            key: 'antd-uploader',
            type: 'component',
            component: (
                <Upload
                    {...uploadInfoPara}
                    accept="image/*"
                    showUploadList={false}
                    multiple={true}
                    onChange={onUploadChange}
                >
                    <button type="button" className="control-item button upload-button" data-title="插入图片">
                        插入图片
                    </button>
                </Upload>
            )
        }
    ]
    return (
        <BraftEditor
            style={{ border: "1px solid #d9d9d9", borderRadius: "6px" }}
            extendControls={extendControls}
            value={editorStatus}
            onChange={interUpdate}
            {...restProps} />
    )
}


function getRichUploadPara(filename: string) {
    return request<RespData<GetUploadResp>>('/api/p/file/GetEditorUploadParas?name=' + filename);
}


