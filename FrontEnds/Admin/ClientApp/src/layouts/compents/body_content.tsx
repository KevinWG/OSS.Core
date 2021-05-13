import { PageHeaderWrapper } from "@ant-design/pro-layout";
import { Content } from "antd/lib/layout/layout";
import React from "react";

const BodyContent = (prop:any) =>{

    return (   <>
        <PageHeaderWrapper>
        </PageHeaderWrapper>
        <Content style={{marginTop:20,backgroundColor:"white",padding:20}}>
            {prop.children}
        </Content>    
        </>
      );
      
}  
  export default BodyContent;