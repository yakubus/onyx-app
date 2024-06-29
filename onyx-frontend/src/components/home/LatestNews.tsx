import NewsImage1 from "@/assets/images/news1.jpg";
import NewsImage2 from "@/assets/images/news2.jpg";
import NewsImage3 from "@/assets/images/news3.jpg";

const LatestNews = () => {
    return (
        <div className="h-auto max-w-1440px w-full bg-white w-full">
            <p className="text-3xl font-bold mt-16 pt-16 mb-8 w-full text-center">Latest News & Events</p>
            <p className="w-3/5 mx-auto block text-center">On the other hand we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment so blinded. Lorem ipsum dolor sit amet consectetur.</p>
            <div className="h-auto max-w-1440px  w-full bg-white py-16 relative flex justify-between flex-wrap flex-col md:flex-row">
                <div className="min-h-52 w-11/12 md:w-30% shadow-md hover:shadow-lg cursor-pointer p-6 mx-auto md:mx-0 mb-6 md:mb-0 ">
                    <div>
                        <img src={NewsImage1} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4 ">What is Cryptocurrency?</p>                 
                    <p className="mt-4 text-xs">June, 2024</p>
                </div>                
                <div className="min-h-52 w-11/12 md:w-30% shadow-md hover:shadow-lg cursor-pointer p-6 mx-auto md:mx-0 mb-6 md:mb-0 ">
                    <div>
                        <img src={NewsImage2} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4">Cryptocurrency Space</p>              
                    <p className="mt-4 text-xs">June, 2024</p>
                </div>
                <div className="min-h-52 w-11/12 md:w-30% shadow-md hover:shadow-lg cursor-pointer p-6 mx-auto md:mx-0  mb-6 md:mb-0">
                    <div>
                        <img src={NewsImage3} alt="Icon card"/>
                    </div>
                    <p className="text-lg font-semibold mt-4 ">Investment Portfolio</p>                  
                    <p className="mt-4 text-xs">June, 2024</p>
                </div>
            </div>
      </div>
    );
};
export default LatestNews;