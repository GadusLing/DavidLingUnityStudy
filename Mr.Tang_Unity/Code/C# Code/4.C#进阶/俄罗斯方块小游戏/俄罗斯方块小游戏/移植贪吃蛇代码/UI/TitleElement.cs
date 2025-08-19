using System;

namespace ����˹����.Main.UI
{
    /// <summary>
    /// ����UIԪ�� - ������ʾ�����������Ҫ�ı�
    /// ְ��
    /// 1. ������ʾ�����ı�
    /// 2. ʹ�ù̶��İ�ɫ�ı���ɫ
    /// 3. �ṩ���õ��Ӿ���θ�
    /// �ص㣺��չʾ��Ԫ�أ���֧�ֽ���
    /// ��;���������⡢��Ҫ��Ϣչʾ
    /// </summary>
    public class TitleElement : IUIElement
    {
        #region ˽���ֶ�
        /// <summary>
        /// �����ı����� - Ҫ��ʾ�ı�������
        /// </summary>
        private string _title;
        #endregion
        
        #region ���캯��
        /// <summary>
        /// ����Ԫ�ع��캯��
        /// </summary>
        /// <param name="title">Ҫ��ʾ�ı����ı�</param>
        public TitleElement(string title)
        {
            _title = title;
        }
        #endregion

        #region IUIElement�ӿ�ʵ��
        /// <summary>
        /// ��Ⱦ���⵽����̨
        /// ���Ĺ��ܣ�
        /// 1. ���ð�ɫ�ı���ɫ������̶�Ϊ��ɫ��
        /// 2. �������λ��
        /// 3. ��������ı�
        /// ע�⣺���ⲻ��Ӧѡ��״̬��isSelected����������
        /// </summary>
        /// <param name="centerX">��Ļ����X���꣬���ھ��ж���</param>
        /// <param name="startY">�����Y����λ��</param>
        /// <param name="isSelected">ѡ��״̬�����ⲻ֧��ѡ�У��˲��������ԣ�</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // ����̶�ʹ�ð�ɫ������ѡ��״̬Ӱ��
            Console.ForegroundColor = ConsoleColor.White;
            
            // �������λ�ã�ʹ��UIHelperȷ����Ӣ�Ļ�ϱ���ľ�ȷ����
            Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_title) / 2, startY);
            
            // ��������ı�
            Console.WriteLine(_title);
        }

        /// <summary>
        /// ��ȡ����߶�
        /// ����̶�ռ��1�и߶�
        /// </summary>
        /// <returns>����߶ȣ�1�У�</returns>
        public int GetHeight() => 1;
        #endregion
    }
}