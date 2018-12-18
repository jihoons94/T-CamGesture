using System;

public interface IContent {

    KidsError LoadContent(KContent content);
    void UnloadContent();

    void SetTime(float time);

    void Play();
    void Pause();
    void Stop();

    void Show();
    void Hide();
}