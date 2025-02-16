﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interact
{
    public class InteractionController : MonoBehaviour
    {
        #region Variables
            [Header ("Data")]
            public InteractionInputData interactionInputData;
            public InteractionData interactionData;

            [Space]
            [Header ("Ray Settings")]
            public float rayDistance;
            public float raySphereRadius;
            public LayerMask interactibleLayer;

            #region Private
                private Camera m_Camera;

                private bool m_interacting;
                private float m_holdTimer = 0f;
            #endregion

        #endregion
        
        #region Built-in methods
            void Awake()
            {
                m_Camera = FindObjectOfType<Camera>();
            }

            void Update()
            {
                CheckForInteractible(); // checks if player is pointing at interactible
                CheckForInteractibleInput();    // if it is an interactible, do something
            }
        #endregion

        #region Custom Methods
            void CheckForInteractible()
            {
                Ray _ray = new Ray(m_Camera.transform.position, m_Camera.transform.forward);
                RaycastHit _hitInfo;

                bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactibleLayer);

                if (_hitSomething) // self explanatory, if the ray hits something
                {
                    InteractibleBase _interactible = _hitInfo.transform.GetComponent<InteractibleBase>(); // checks if interacble has the component interactible base on it

                    if(_interactible != null) // if the interactible is not null
                    {
                        if(interactionData.IsEmpty()) // if the interactible data is empty / if there is a slot for an interactible
                        {
                            interactionData.Interactible = _interactible; // interaction data is set to this new interactible
                        }
                        else // if there is an interactible in the interactible slot
                        {
                            if(!interactionData.isSameInteractible(_interactible)) // check if its not the same
                            {
                                interactionData.Interactible = _interactible; // override the current interactible data
                            }
                        }
                    }
                }
                else // if we don't hit anything
                {
                    interactionData.ResetData();
                }

                Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);
            }

            void CheckForInteractibleInput()
            {
                if(interactionData.IsEmpty())
                    return;
                 

                if (interactionInputData.InteractClicked)
                {
                    m_interacting = true;
                    m_holdTimer = 0f;
                }

                if(interactionInputData.InteractReleased)
                {
                    m_interacting = false;
                    m_holdTimer = 0f;
                }

                if(m_interacting)
                {
                    if(!interactionData.Interactible.IsInteractible)
                        return;
                    

                    if(interactionData.Interactible.HoldInteract)
                    {
                        m_holdTimer += Time.deltaTime;

                        if(m_holdTimer >= interactionData.Interactible.HoldDuration)
                        {
                            interactionData.Interact();
                            m_interacting = false;
                        }
                    }
                    else
                    {
                        interactionData.Interact();
                        m_interacting = false;
                    }
                }
                
            }
        #endregion
    }
}
