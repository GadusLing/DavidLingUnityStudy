using System;

namespace ����˹����.Main.UI
{
    /// <summary>
    /// UIԪ�ؽӿ� - ��������UI�����ͨ����Ϊ
    /// ���ã�ͳһUIԪ�ص���Ⱦ�淶��ʵ�ֶ�̬��Ⱦ
    /// ���ģʽ������ģʽ�Ľӿڶ���
    /// ʵ���ࣺTitleElement��ButtonElement��TextContentElement��SpacerElement
    /// </summary>
    public interface IUIElement
    {
        /// <summary>
        /// ��ȾUIԪ�ص�����̨
        /// </summary>
        /// <param name="centerX">��Ļ����X���꣬���ھ��ж���</param>
        /// <param name="startY">Ԫ����ʼY����</param>
        /// <param name="isSelected">�Ƿ���ѡ��״̬��Ӱ����ʾ��ɫ�ȣ�</param>
        void Render(int centerX, int startY, bool isSelected);
        
        /// <summary>
        /// ��ȡUIԪ�صĸ߶ȣ�ռ�õ�������
        /// ���ڼ�����һ��Ԫ�ص���Ⱦλ��
        /// </summary>
        /// <returns>Ԫ�ظ߶ȣ�������</returns>
        int GetHeight();
    }
}