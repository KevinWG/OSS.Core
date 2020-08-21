import React from 'react';
import RightContent from './compents/right_content';
import Footer from './compents/footer';

export default {
  rightRender: () => {
    return <RightContent />;
  },
  disableContentMargin: false,
  footerRender: () => <Footer />,
};
