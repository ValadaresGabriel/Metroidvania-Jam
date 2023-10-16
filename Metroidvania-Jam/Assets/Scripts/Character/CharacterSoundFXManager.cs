using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterSoundFXManager : MonoBehaviour
    {
        private AudioSource audioSource;

        public LayerMask terrainLayer;
        public LayerMask obstacleLayer;
        private string currentMaterial; // Current material the player is on
        private Terrain terrain;
        private Dictionary<string, TerrainSounds> materialSoundMap;

        protected virtual void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected virtual void Start()
        {
            materialSoundMap = new Dictionary<string, TerrainSounds>()
            {
                { "Water", WorldSoundFXManager.Instance.grassSounds },
                { "Grass", WorldSoundFXManager.Instance.grassSounds },
                { "Road", WorldSoundFXManager.Instance.rockRoadSounds },
                { "Dirt", WorldSoundFXManager.Instance.dirtSounds },
                { "Earth", WorldSoundFXManager.Instance.dirtSounds },
                { "Sand", WorldSoundFXManager.Instance.sandSounds },
                { "Wood", WorldSoundFXManager.Instance.rockRoadSounds },
                { "Rock", WorldSoundFXManager.Instance.rockRoadSounds },
            };

            FindTerrain();
        }

        public void FindTerrain()
        {
            terrain = GameObject.FindWithTag("Terrain").GetComponent<Terrain>();
        }

        public void PlayFootstepSound()
        {
            DetectMaterial();
            AudioClip[] selectedAudioClip = null;
            TerrainSounds terrainSounds;

            foreach (var key in materialSoundMap.Keys)
            {
                if (currentMaterial.Contains(key))
                {
                    terrainSounds = materialSoundMap[key];
                    selectedAudioClip = PlayerInputManager.Instance.moveAmount <= 0.5f ?
                                        terrainSounds.walk :
                                        terrainSounds.run;
                    break;
                }
            }

            if (selectedAudioClip != null)
            {
                audioSource.PlayOneShot(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(selectedAudioClip));
            }
        }

        private void DetectMaterial()
        {
            RaycastHit hit;
            if (terrain != null && Physics.Raycast(transform.position, Vector3.down, out hit, 1f, terrainLayer))
            {
                int textureIndex = GetMainTexture(transform.position, terrain);
                currentMaterial = terrain.terrainData.terrainLayers[textureIndex].name;
            }
            else if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f, obstacleLayer))
            {
                currentMaterial = hit.collider.gameObject.tag;
            }
        }

        private int GetMainTexture(Vector3 worldPos, Terrain terrain)
        {
            TerrainData terrainData = terrain.terrainData;
            Vector3 terrainPos = terrain.transform.position;

            int mapX = (int)(((worldPos.x - terrainPos.x) / terrainData.size.x) * terrainData.alphamapWidth);
            int mapZ = (int)(((worldPos.z - terrainPos.z) / terrainData.size.z) * terrainData.alphamapHeight);

            float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);
            float[] cellMix = new float[splatmapData.GetUpperBound(2) + 1];

            for (int n = 0; n < cellMix.Length; ++n)
            {
                cellMix[n] = splatmapData[0, 0, n];
            }

            int mostProminentTexture = 0;
            float maxMix = 0;
            for (int n = 0; n < cellMix.Length; ++n)
            {
                if (cellMix[n] > maxMix)
                {
                    mostProminentTexture = n;
                    maxMix = cellMix[n];
                }
            }

            return mostProminentTexture;
        }

        public void PlaySoundFX(AudioClip soundFX, float volume = 1f, bool randomizePitch = true, float pitchRandom = 0.1f)
        {
            audioSource.PlayOneShot(soundFX, volume);
            // Resets Pitch
            audioSource.pitch = 1;

            if (randomizePitch)
            {
                audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
            }
        }

        public void PlayRollSoundFX()
        {
            audioSource.PlayOneShot(WorldSoundFXManager.Instance.rollSFX);
        }
    }
}
