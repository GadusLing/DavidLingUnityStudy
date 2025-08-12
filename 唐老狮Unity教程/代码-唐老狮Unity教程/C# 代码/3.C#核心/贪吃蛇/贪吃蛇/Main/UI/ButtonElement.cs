using System;

namespace ̰����.Main.UI
{
    /// <summary>
    /// ��ťUIԪ�� - �ɽ����İ�ť���
    /// ְ��
    /// 1. ��ʾ��ť�ı���֧��ѡ��״̬����ɫ�仯��
    /// 2. ִ���û�����Ĳ�����ͨ��Actionί�У�
    /// 3. ֧�ּ��̵����ͽ���
    /// �ص㣺Ψһ��ִ�в�����UIԪ��
    /// ʵ�֣�IUIElement�ӿڣ�֧�ֶ�̬��Ⱦ
    /// </summary>
    public class ButtonElement : IUIElement
    {
        #region ˽���ֶ�
        /// <summary>
        /// ��ť��ʾ�ı� - �û������İ�ť��ǩ
        /// </summary>
        private string _text;
        
        /// <summary>
        /// ��ť���ʱִ�еĲ��� - ʹ��ί��ʵ�ֻص�����
        /// ֧�������޲����Ĳ������糡���л�����Ϸ�˳���
        /// </summary>
        private Action _action;
        #endregion

        #region ���캯��
        /// <summary>
        /// ��ťԪ�ع��캯��
        /// </summary>
        /// <param name="text">��ť��ʾ���ı�����</param>
        /// <param name="action">��ť������ʱִ�еĲ�����ί�У�</param>
        public ButtonElement(string text, Action action)
        {
            _text = text;
            _action = action;
        }
        #endregion

        #region IUIElement�ӿ�ʵ��
        /// <summary>
        /// ��Ⱦ��ť������̨
        /// ���Ĺ��ܣ�
        /// 1. ����ѡ��״̬�����ı���ɫ��ѡ��=��ɫ��δѡ��=��ɫ��
        /// 2. �������λ�ò����ù������
        /// 3. �����ť�ı�
        /// </summary>
        /// <param name="centerX">��Ļ����X���꣬���ھ��ж���</param>
        /// <param name="startY">��ť��Y����λ��</param>
        /// <param name="isSelected">�Ƿ�Ϊ��ǰѡ�еİ�ť</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // �����ı���ɫ��ѡ��״̬Ϊ��ɫ����ͨ״̬Ϊ��ɫ
            Console.ForegroundColor = isSelected ? ConsoleColor.Red : ConsoleColor.White;
            
            // �������λ�ã�����X�����ȥ�ı���ȵ�һ��
            // ʹ��UIHelper.GetDisplayWidthȷ����Ӣ�Ļ���ı��ľ�ȷ����
            Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_text) / 2, startY);
            
            // �����ť�ı�
            Console.WriteLine(_text);
        }

        /// <summary>
        /// ��ȡ��ť�߶�
        /// ��ť�̶�ռ��1�и߶�
        /// </summary>
        /// <returns>��ť�߶ȣ�1�У�</returns>
        public int GetHeight() => 1;
        #endregion

        #region ��ť���й���
        /// <summary>
        /// ִ�а�ť���� - ��ť������ʱ����
        /// ʹ��?.���������п�ֵ��飬��ֹ�������쳣
        /// ����ʱ�����û�����Enter���ҵ�ǰ��ť��ѡ��ʱ
        /// </summary>
        public void Execute() => _action?.Invoke();
        #endregion
    }
}