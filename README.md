Autoria de:
Jennifer Silva, 31086
Giulia Souza, 27553

# Unity 3D - Trilha na Floresta
## Versão do Unity
- **Unity**: 6000.3.9f1

## Descrição do projeto
Este projeto consiste num jogo desenvolvido em Unity, com um ciclo de jogabilidade baseado em evitar obstáculos, acumular pontuação ao longo do tempo e reiniciar após Game Over.

- **Resumo do jogo**: O jogador controla a personagem num percurso com obstáculos. O objetivo é sobreviver o máximo de tempo possível, acumulando pontos enquanto a dificuldade aumenta progressivamente. Ao colidir com um obstáculo, ocorre Game Over e é possível reiniciar.
- **Funcionalidades implementadas**:
  - **Menu principal** com navegação entre cenas:
    - `MainMenu` (menu)
    - `SampleScene` (jogo)
    - `About` (sobre)
  - **Sistema de pontuação e recorde**:
    - Pontuação baseada no tempo decorrido
    - Recorde guardado com `PlayerPrefs` (HighScore)
  - **Contagem regressiva antes do início** (3, 2, 1, “JÁ!”)
  - **Dificuldade incremental**:
    - A velocidade do cenário/obstáculos aumenta ao longo do tempo (até um máximo)
    - O intervalo de spawn de obstáculos diminui com o tempo (com limite mínimo)
  - **Game Over e reinício**:
    - Mostra painel de Game Over
    - Exibe pontuação final
    - Permite reiniciar a cena
  - **Sistema de pausa**:
    - Tecla `ESC` abre/fecha menu de pausa e congela o tempo (`Time.timeScale`)
  - **Movimento por faixas (lanes)**:
    - Troca de faixa esquerda/centro/direita
  - **Salto**:
    - Salto ao pressionar `Espaço` quando no chão
    - Ajuste de gravidade (queda mais rápida) para “feel” melhor do salto
  - **Áudio (música e efeitos)**:
    - Música ambiente
    - Efeitos de salto, botão/clique e game over

## Jogabilidade (Como jogar)
- **Objetivo**: Obter a maior pontuação possível sobrevivendo o máximo de tempo sem colidir com obstáculos.
- **Controlos**:
  - **Espaço**: saltar (quando a personagem está no chão)
  - **A** ou **←**: mudar de faixa (esquerda/direita conforme configuração interna)
  - **D** ou **→**: mudar de faixa (esquerda/direita conforme configuração interna)
  - **ESC**: pausar/retomar o jogo (abre/fecha menu de pausa)
- **Regras**:
  - **Colidir com um obstáculo** termina a ronda (Game Over).
  - **Pontuação** aumenta automaticamente com o tempo.
  - A **dificuldade aumenta** ao longo do tempo (velocidade maior e spawns mais frequentes).
- **Condições de vitória/derrota**:
  - **Derrota**: colisão com obstáculo.
  - **Vitória**: não existe “vitória” final — o jogo é do tipo endless e o objetivo é bater o recorde.

## Como abrir e executar o projeto
- **Requisitos**:
  - Unity Hub instalado
  - Unity **6000.3.9f1** instalada no Hub

- **Abrir no Unity Hub**:
  1. Abrir o **Unity Hub**
  2. Selecionar **Add / Open** (Adicionar / Abrir projeto)
  3. Escolher a pasta do projeto `Unity3D/` (onde existem as pastas `Assets/`, `ProjectSettings/`, etc.)
  4. Confirmar que o projeto abre com a versão **6000.3.9f1**

- **Correr o jogo**:
  1. No Unity, abrir a cena inicial: `Assets/Scenes/MainMenu.unity`
  2. Carregar em **Play** (botão ▶ no Editor)
  3. No menu, iniciar o jogo (Play) para ir para `SampleScene`

## Assets multimédia
- SimpleNaturePack: Unity Store

- **Áudio**:
  - **Formatos usados no projeto**: WAV e MP3
  - **Ficheiros presentes (exemplos)**:
    - `Assets/Musics/Happy.wav` (WAV)
    - `Assets/Musics/mixkit-light-button-2580.wav` (WAV)
    - `Assets/Musics/mixkit-little-piano-game-over-1944.wav` (WAV)
    - `Assets/Musics/freesound_community-cartoon-jump-6462.mp3` (MP3)
  - **Justificação**:
    - WAV é adequado para efeitos e música com boa qualidade e sem perdas.
    - MP3 reduz o tamanho do ficheiro, útil para alguns efeitos/áudios sem necessidade de qualidade máxima.
    Fontes: https://mixkit.co/ e https://freesound.org

## Observações / lacunas conhecidas
- **Melhorias futuras sugeridas**:
  - Ecrã de opções com sliders para volume de música
  - Tutorial curto no menu “About” ou antes da primeira ronda