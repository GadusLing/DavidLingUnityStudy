using System;

namespace ̰����.Main.UI
{
    /// <summary>
    /// UI���������� - �ṩUI��ص�ͨ�ù��߷���
    /// ְ�𣺴���UI��Ⱦ�еĳ�����������
    /// ���ģʽ��������ģʽ����̬�������ϣ�
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// �����ı���ʾ��ȣ�֧�������ַ���
        /// �㷨�������ַ����Ϊ2��Ӣ���ַ����Ϊ1
        /// ��;��ʵ���ı��ľ�ȷ���ж���
        /// </summary>
        /// <param name="text">Ҫ�����ȵ��ı�</param>
        /// <returns>�ı�����ʾ��ȣ��ַ���λ��</returns>
        public static int GetDisplayWidth(string text)
        {
            int width = 0;
            foreach (char c in text)
            {
                // �ж��Ƿ�Ϊ�����ַ���Unicode��Χ��4E00-9FFF��
                if (c >= 0x4E00 && c <= 0x9FFF) // �����ַ�
                    width += 2;
                else                             // Ӣ���ַ�
                    width += 1;
            }
            return width;
        }
    }
}