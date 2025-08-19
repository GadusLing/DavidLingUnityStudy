using System;

namespace ̰����.Main.UI
{
    /// <summary>
    /// �ı�����UIԪ�� - ������ʾ�����ı�����
    /// ְ��
    /// 1. ֧�ֶ����ı��ľ�����ʾ
    /// 2. �Զ���������Ĵ�ֱ�ռ�
    /// 3. �ṩ��Ϣչʾ����
    /// �ص㣺
    /// - ֧�ֿɱ�������죨�ɴ��������������ı���
    /// - ÿ���ı��������ж���
    /// - ��չʾ��Ԫ�أ���֧�ֽ���
    /// ��;��������Ա��������Ϸ˵����������ʾ��Ϣ��
    /// ʵ�֣�IUIElement�ӿڣ�֧�ֶ�̬��Ⱦ
    /// </summary>
    public class TextContentElement : IUIElement
    {
        #region ˽���ֶ�
        /// <summary>
        /// �ı������� - �洢����Ҫ��ʾ���ı���
        /// ÿ��Ԫ�ش���һ���ı�����
        /// </summary>
        private string[] _lines;
        #endregion

        #region ���캯��
        /// <summary>
        /// �ı�����Ԫ�ع��캯��
        /// ʹ�ÿɱ������֧�ִ��������������ı���
        /// </summary>
        /// <param name="lines">Ҫ��ʾ���ı��У��ɱ������</param>
        /// <example>
        /// ʹ��ʾ����
        /// new TextContentElement("��һ��", "�ڶ���", "������");
        /// new TextContentElement("�߻�:David", "����:David");
        /// </example>
        public TextContentElement(params string[] lines)
        {
            _lines = lines;
        }
        #endregion

        #region IUIElement�ӿ�ʵ��
        /// <summary>
        /// ��Ⱦ�����ı����ݵ�����̨
        /// ���Ĺ��ܣ�
        /// 1. ���ð�ɫ�ı���ɫ���̶���ɫ������ѡ��״̬Ӱ�죩
        /// 2. ������Ⱦ��ÿ�ж����������λ��
        /// 3. �Զ�����ֱλ�õ���
        /// �㷨��
        /// - ÿ���ı�������centerX���о��м���
        /// - Y�����startY��ʼ��ÿ�е���1
        /// - ʹ��UIHelper.GetDisplayWidthȷ����Ӣ�Ļ���ı��ľ�ȷ����
        /// </summary>
        /// <param name="centerX">��Ļ����X���꣬����ÿ�еľ��ж���</param>
        /// <param name="startY">��һ���ı���Y����λ��</param>
        /// <param name="isSelected">ѡ��״̬���ı����ݲ�֧��ѡ�У��˲��������ԣ�</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // �ı����ݹ̶�ʹ�ð�ɫ������ѡ��״̬Ӱ��
            // ����һ�µ��Ӿ�Ч��
            Console.ForegroundColor = ConsoleColor.White;
            
            // ������Ⱦ�ı�����
            for (int i = 0; i < _lines.Length; i++)
            {
                // Ϊÿ�ж����������λ��
                // centerX - (�ı���ʾ��� / 2) = �ı���ʼX����
                // startY + i = ��ǰ�е�Y����
                Console.SetCursorPosition(centerX - UIHelper.GetDisplayWidth(_lines[i]) / 2, startY + i);
                
                // �����ǰ���ı�
                Console.WriteLine(_lines[i]);
            }
        }

        /// <summary>
        /// ��ȡ�ı����ݵ��ܸ߶�
        /// �߶ȼ��㣺�ı����� = ռ�õ���ʾ����
        /// ���ں���UIԪ�ص�λ�ü���
        /// </summary>
        /// <returns>�ı����ݵ�������</returns>
        public int GetHeight() => _lines.Length;
        #endregion
    }
}