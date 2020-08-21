import React from 'react';
import { Drawer, Divider, Col, Row, Avatar } from 'antd';
import { DrawerProps } from 'antd/lib/drawer';
import { UserInfo, user_status } from '../data_d';
import moment from 'moment';

interface ProfileProps extends DrawerProps {
  user: UserInfo;
}

const DescriptionItem = ({ title, content }: { title: any; content: any }) => (
  <Row align={'middle'}>
    <Col span={8} style={{ textAlign: 'right' }}>
      <strong>{title}</strong>
    </Col>
    <Col span={16}>{content}</Col>
  </Row>
);

const Profile = (props: ProfileProps) => {
  return (
    <Drawer title="个人详情" placement="right" width={640} {...props}>
      <Row justify={'center'} style={{ marginTop: 20 }}>
        <Avatar size={80} src={props.user.avatar} />
      </Row>
      <Divider />
      <Row style={{ marginBottom: 10 }}>
        <Col span={12}>
          <DescriptionItem title="昵称：" content={props.user.nick_name} />
        </Col>
        <Col span={12}>
          <DescriptionItem title="邮箱：" content={props.user.email} />
        </Col>
      </Row>
      <Row style={{ marginBottom: 10 }}>
        <Col span={12}>
          <DescriptionItem
            title="注册时间："
            content={moment(props.user.add_time * 1000).format('YYYY-MM-DD')}
          />
        </Col>
        <Col span={12}>
          <DescriptionItem title="状态：" content={user_status[props.user.status]?.text} />
        </Col>
      </Row>
    </Drawer>
  );
};

export default Profile;
