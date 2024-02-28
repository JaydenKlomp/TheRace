using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DragRacerPrep
{
    internal class csRacerBase
    {
        #region beschikbare variabelen
        int tunedRacerMaxSpeed = 0; //willekeurige waarde om snelheid te bepalen
        double stepSizePerTick = 0;
        double actualPositionInLane = 0; //positie van de racer, niet afgerond, dit is dus de ECHTE positie
        int actualPositionInLaneRounded; //afgerond omdat posities van form items met pixels werken
        public string racerName = ""; //IPC4
        #endregion

        #region variabelen en methoden nog nodig
        //racer is gefinished, bool IPC 5,1
        public bool racerFinished = false;
        //reset finished racer, setter
        #endregion

        #region instellen
        /// <summary>
        /// instellen snelheid van een racer: neem default tussen 500 en 800
        /// </summary>
        /// <param name="a_minValue"></param>
        /// <param name="a_maxValue"></param>
        public void setSpeed(int a_speedWithBadTuning, int a_speedWithGoodTuning)
        {

            Random generateRacerSpeed = new Random();

            tunedRacerMaxSpeed = generateRacerSpeed.Next(a_speedWithBadTuning, a_speedWithGoodTuning);

            ResetSpeedSettings();

            //return m_setupCounter.ToString();
        }


        /// <summary>
        /// Use to reset setSpeed function; if NOT used, the same speed is generated for every racer
        /// </summary>
        /// <returns></returns>
        private void ResetSpeedSettings()
        {
            int m_setupCounter = 0;

            for (int i = 0; i < 12345678; i++)
            {
                m_setupCounter++;
            }
        }

        /// <summary>
        /// de stapgrootte bij elke timer tick, berekend op basis van de baanlengte
        /// </summary>
        /// <param name="a_laneLengthToFinishLine"></param>
        public void calculateStepSize(double a_laneLengthToFinishLine)
        {
            stepSizePerTick = tunedRacerMaxSpeed / a_laneLengthToFinishLine;
        }

        /// <summary>
        /// elke tick wordt berekend waar de racer is, in echte pixels
        /// </summary>
        /// <param name="a_stepsPerTick"></param>
        public void calculateCurrentPosition()
        {
            //positie in doubles berekenen
            //met de volledige getallen(inclusief decimalen) rekenen
            //eerst doubles optellen, dan pas omzetten naar een int voor de echte positie
            actualPositionInLane = actualPositionInLane + stepSizePerTick;

            //automatische afronding
            actualPositionInLaneRounded = Convert.ToInt32(actualPositionInLane);
        }

        public void calculateBackwardsPositionJKLO()
        {
            actualPositionInLane = actualPositionInLane - stepSizePerTick;

            actualPositionInLaneRounded = Convert.ToInt32(actualPositionInLane);
        }
        /// <summary>
        /// echte positie van de racer, de positie van de picturebox is hiervan afgeleid
        /// </summary>
        /// <param name="a_startPosition"></param>
        public void resetPositionJKLO(int a_startPosition)
        {
            actualPositionInLane = a_startPosition;
        }
        #endregion

        #region ophalen gegevens van de racer

        /// <summary>
        /// wat is de max snelheid bij de huidige motor tuning
        /// </summary>
        public int GetTunedRacerSpeed
        {
            get
            {
                return tunedRacerMaxSpeed;
            }
        }

        /// <summary>
        /// aantal stappen bij elke timer tick
        /// </summary>
        public double GetStepSizePerTick
        {
            get
            {
                return stepSizePerTick;
            }
        }

        /// <summary>
        /// positie na het huidige aantal timer ticks, dus b.v. 10 ticks is 10 stappen van x afstand
        /// </summary>
        public int GetActualPositionInLaneRounded
        {
            get
            {
                return actualPositionInLaneRounded;
            }
        }
        #endregion
    }
}