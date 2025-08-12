using System;

namespace ̰����.Main.UI
{
    /// <summary>
    /// �հ׼��UIԪ�� - ���ڿ���UIԪ��֮��ļ��
    /// ְ��
    /// 1. ��UIԪ��֮�䴴���ɿصĿհ׿ռ�
    /// 2. �ṩ���Ĳ��ֿ�������
    /// 3. ����̶���಼�ֵ���������
    /// �ص㣺
    /// - ����Ⱦ�κοɼ�����
    /// - ���Զ���ռ�õ�����
    /// - ��С�߶�Ϊ1�У���ֹ��Ч��ࣩ
    /// ������壺ʵ����"��༴���"���������
    /// </summary>
    public class SpacerElement : IUIElement
    {
        #region ˽���ֶ�
        /// <summary>
        /// ���߶� - �ü��Ԫ��ռ�õ�����
        /// ��СֵΪ1��ȷ�������Ч��
        /// </summary>
        private int _height;
        #endregion

        #region ���캯��
        /// <summary>
        /// ���Ԫ�ع��캯��
        /// </summary>
        /// <param name="height">���߶ȣ���������Ĭ��Ϊ1�У���СֵΪ1</param>
        public SpacerElement(int height = 1)
        {
            // ȷ���������Ϊ1�У���ֹ��Ч���
            _height = Math.Max(1, height);
        }
        #endregion

        #region IUIElement�ӿ�ʵ��
        /// <summary>
        /// ��Ⱦ�հ׼�ࣨʵ���ϲ���Ⱦ�κ����ݣ�
        /// �������ͨ��"����Ⱦ"��ʵ�ֿհ�Ч��
        /// ��������ͨ��GetHeight()����Ӱ�����Ԫ�ص�λ��ʵ��
        /// </summary>
        /// <param name="centerX">����X���꣨���Ԫ�ز�ʹ�ã�</param>
        /// <param name="startY">��ʼY���꣨���Ԫ�ز�ʹ�ã�</param>
        /// <param name="isSelected">ѡ��״̬�����Ԫ�ز�֧��ѡ�У�</param>
        public void Render(int centerX, int startY, bool isSelected)
        {
            // ���Ԫ�صĺ����ص㣺����Ⱦ�κοɼ�����
            // ͨ��"��ʵ��"�ﵽ�����հ׿ռ��Ŀ��
        }

        /// <summary>
        /// ��ȡ���߶�
        /// ���ع���ʱ���õ����������ڲ��ּ���
        /// </summary>
        /// <returns>���ռ�õ�����</returns>
        public int GetHeight() => _height;
        #endregion
    }
}