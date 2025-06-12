# 👾 Proyecto Toly  – Versión 3 “Final" 📚

**Descripción:**  
Juego de plataformas 2D en Unity, desarrollado como proyecto de la materia de Implantación de la Licenciatura en Informática y Tecnologías Computacionales (UAA, octavo semestre). Controlas a Toly, evitas trampas, derrotas enemigos y coleccionas corazones para seguir vivo.

---

## 🚀 Cómo arrancar

1. **Clona el repo**  
   ```bash
   git clone https://github.com/Delariva666/PROYECTO-TOLY-3.git

2. Abre Unity Hub y agrega la carpeta del proyecto (Unity 2020.3 LTS o superior).


3. Carga la escena

Ve a Assets > Scenes > Nivel1.unity y haz clic en Play.





---

📂 Estructura

PROYECTO-TOLY-3/
├─ Assets/
│  ├─ Scenes/            ← Nivel1.unity, Nivel2.unity
│  ├─ Scripts/           ← Lógica de movimiento, salud y UI
│  ├─ Prefabs/           ← Heart.prefab, DeathZone.prefab
│  └─ Sprites/           ← Gráficos de fondo y personajes
├─ ProjectSettings/      ← TagManager (Player, Ground, Enemy, Hazard…)
└─ README.md             ← Este documento


---

🎮 Controles

Mover: A/D o ←/→

Saltar: Space

Atacar: c (al enemigo por detrás)



---

💻 Principales scripts

Mover.cs: velocidad, salto y física básica.

Collisiones.cs: vida, daño, invulnerabilidad y respawn.

PlayerHit.cs: destruye enemigos al contacto.

HeartManager.cs: muestra corazones según la vida.

HealthSlider.cs: sincroniza barra de salud con la vida actual.



---

🔄 Extender

Nuevos niveles: duplicar y editar escenas.

Más enemigos: crear prefab con PlayerHit.cs.

UI extra: añadir Canvas con puntuación, temporizador, etc.



---

🤝 Contribuir

1. Abre un issue para bugs o ideas.


2. Haz fork y rama (feature/tu-idea).


3. Envía pull request con descripción clara.




---

🛣️ Roadmap

[ ] Nivel 3 con plataformas móviles

[ ] Jefes con patrones de ataque

[ ] Sistema de puntos y monedas

[ ] Guardado de progreso local



---

📄 Licencia

MIT License. Usa, modifica y comparte sin problema.


