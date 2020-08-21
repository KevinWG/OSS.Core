import React from 'react';
import { Card, Row, Col } from 'antd';
import AvatarWithUpload from './avatar_with_upload';
import { AdminInfo } from '../data_d';

const AdminDetail: React.FC<{ admininfo: AdminInfo; is_mine: boolean }> = ({
  admininfo,
  is_mine,
}) => {
  return (
    <Card>
      <Row>
        <Col span={6} style={{ textAlign: 'center' }}>
          <AvatarWithUpload is_mine={is_mine} />
        </Col>
        <Col span={18}>
          <Card title="基础信息" bordered={false}>
            <Row>
              <Col span={4} style={{ textAlign: 'right' }}>
                姓名：
              </Col>
              <Col span={20}>{admininfo.admin_name}</Col>
            </Row>
          </Card>
        </Col>
      </Row>
    </Card>
  );
};

export default AdminDetail;
