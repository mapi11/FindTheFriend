mergeInto(LibraryManager.library, {
    SetupVisibilityListener: function () {
        document.addEventListener("visibilitychange", function() {
            if (document.hidden) {
                // �������� ������ (����������� ������� ��� �������� �������)
                unityInstance.SendMessage('AudioManager', 'OnPageHidden');
            } else {
                // �������� ����� �������
                unityInstance.SendMessage('AudioManager', 'OnPageVisible');
            }
        });
    }
});