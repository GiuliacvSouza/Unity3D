Autoria de:
Jennifer Silva, 31086
Giulia Souza, 27553

# Unity 3D - Trilha na Floresta
<img width="1756" height="974" alt="Captura de ecrã 2026-04-24 190633" src="https://github.com/user-attachments/assets/23da38a6-141f-43ad-adc9-9e0d1e2e1bce" />

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

## Arquitetura do sistema

### Visão geral
O projeto está organizado em **cenas** e **scripts C# (MonoBehaviours)** que implementam a lógica de UI, áudio e comportamentos no mundo 3D.

- **Cenas principais**
  - **`MainMenu`**: UI de menu (botões com feedback visual/sonoro).
  - **`SampleScene`**: gameplay (corrida/endless runner, pontuação, obstáculos, etc.).
  - **`About`**: ecrã informativo.

- **Componentes “globais”**
  - **`AudioManager`** funciona como **Singleton persistente** (não é destruído ao trocar de cena) e centraliza:
    - Música ambiente
    - Efeitos sonoros (SFX)
    - Controlo de volume e transições (ex.: fade out)

- **Camada de UI (Menu)**
  - Scripts ligados a elementos UI (Button/Text/Image) para **hover**, **realce** e **som de clique**.

- **Camada de mundo 3D / Ambiente**
  - Scripts de “ajuste” geométrico e utilitários (ex.: conformar bordas de uma malha ao terreno via raycasts)

## Explicação técnica dos scripts

### `Assets/Scripts/AudioManager.cs`
- **Padrão**: Singleton via `public static AudioManager Instance` no `Awake()`, com `DontDestroyOnLoad(gameObject)` para persistir entre cenas.
- **Fontes de áudio**:
  - `musicSource`: dedicado a música (clip em loop).
  - `sfxSource`: dedicado a efeitos (toca via `PlayOneShot`).
- **Gestão por cena**:
  - Subscreve `SceneManager.sceneLoaded` em `OnEnable()` e remove em `OnDisable()`.
  - Ao carregar uma cena, reinicia a música e também inicia um `Coroutine` com um pequeno `WaitForSecondsRealtime(0.1f)` para evitar conflitos de timing com `Start()` de outros scripts.
- **API pública (usada por outros scripts)**:
  - `TocarSFX(AudioClip clip)` e wrappers `TocarPulo()`, `TocarGameOver()`, `TocarBotao()`
  - `SetVolumeMusica(float)`, `SetVolumeSFX(float)`
  - `PararMusica()`, `PausarMusica()`, `RetomarMusica()`
  - `FadeOutMusica(float duracao)` com `Coroutine` que faz `Mathf.Lerp` do volume até 0 e depois dá `Stop()`.

### `Assets/Scripts/MainMenu/MenuButtonEffect.cs`
- **Objetivo**: feedback visual e sonoro nos botões do menu.
- **Integração UI**:
  - Usa `TextMeshProUGUI` para o texto (`buttonText`) e `UnityEngine.UI.Image` para um “fundo” (`backgroundBox`).
  - Implementa `IPointerEnterHandler`/`IPointerExitHandler` para detetar hover.
- **Comportamento**:
  - No `Start()`: obtém o `Button` e regista `button.onClick.AddListener(TocarSomClique)`.
  - Garante o fundo invisível via `backgroundBox.canvasRenderer.SetAlpha(0.0f)` e força alpha do texto a 1.
  - **Hover enter**: altera cor do texto e torna o fundo visível (alpha 1) com `boxHighlightColor`.
  - **Hover exit**: repõe cor normal do texto e esconde o fundo (alpha 0).
- **Áudio**:
  - No clique, chama `AudioManager.Instance.TocarSFX(somClique)` (se existir `AudioManager` e o clip estiver atribuído).

### `Assets/Scripts/ConformEdgesToGround.cs`
- **Objetivo**: ajustar **apenas os vértices da borda** de uma malha para “encostar” no terreno, evitando gaps/afundamentos visuais.
- **Requisitos**:
  - `[RequireComponent(typeof(MeshFilter))]` garante que o GameObject tem malha.
- **Estratégia**:
  - No `Awake()` duplica a malha (`Instantiate(mf.sharedMesh)`) para **não editar o asset original** e chama `MarkDynamic()`.
  - Calcula `min/max` dos vértices locais para inferir limites.
  - Define um limiar de “borda” com `edgeThresholdNormalized` (percentagem do tamanho em X/Z).
  - Para cada vértice classificado como borda:
    - Converte para mundo (`TransformPoint`)
    - Faz `Physics.Raycast` de cima para baixo (`rayHeight`, `rayDistance`) no `groundLayer`
    - Ajusta `worldPos.y = hit.point.y`
  - Reescreve vértices e recalcula `Normals` e `Bounds`.

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
