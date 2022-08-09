declare namespace NotifyApi {
  interface Template extends BaseMo, AddTemplateReq {}

  interface AddTemplateReq {
    notify_type: number;
    notify_channel: number;
    channel_sender?: string;
    channel_template_code?: string;
    title: string;
    content?: string;
    is_html: number;
    sign_name?: string;
  }
}
