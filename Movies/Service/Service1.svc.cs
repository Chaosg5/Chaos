

namespace Chaos.Movies.Service
{
    using System;
    using System.ServiceModel;
    using Chaos.Movies.Model;

    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public void MovieSave(Movie movie)
        {
            try
            {

            }
            catch (Exception exception)
            {
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public void RatingSave(Rating rating)
        {
            try
            {
                if (rating == null)
                {
                    throw new ArgumentNullException("rating");
                }

                rating.Save();
            }
            catch (Exception exception)
            {
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public void RatingSaveAll(Rating rating)
        {
            try
            {
                if (rating == null)
                {
                    throw new ArgumentNullException("rating");
                }

                rating.SaveAll();
            }
            catch (Exception exception)
            {
                //Logger.Error(exception);
                throw new FaultException(exception.ToString());
            }
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
