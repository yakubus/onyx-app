const AccountsLoadingSkeleton = () => {
  return (
    <div className="h-full animate-pulse overflow-auto px-4 scrollbar-none xl:px-8">
      <div className="grid grid-cols-1 gap-y-10 rounded-xl border bg-card px-4 py-6 shadow-md md:grid-cols-2 md:gap-x-10 md:gap-y-0 lg:gap-x-20">
        <div className="h-44 w-full max-w-[400px] space-y-2 justify-self-center rounded-xl bg-gradient-to-b from-primary via-primary to-primaryDark p-4 shadow-lg shadow-primaryDark/50 lg:justify-self-end" />
        <div className="m-auto max-w-[400px] space-y-10 md:mx-0 md:my-auto md:w-auto md:space-y-4 lg:justify-self-start">
          <div className="flex flex-col space-y-2">
            <div className="h-10 w-36 rounded-md bg-accent"></div>
            <div className="h-2 w-72 rounded-md bg-accent"></div>
          </div>
          <div className="flex space-x-8">
            <div className="flex flex-col space-y-4">
              <div className="h-10 w-32 rounded-md bg-accent"></div>
              <div className="w-42 h-10 rounded-md bg-accent"></div>
            </div>
            <div className="flex flex-col space-y-4">
              <div className="h-10 w-32 rounded-md bg-accent"></div>
              <div className="w-42 h-10 rounded-md bg-accent"></div>
            </div>
          </div>
        </div>
      </div>
      <div className="space-y-4 py-20">
        <div className="overflow-hidden rounded-lg border">
          <div className="h-14 w-full bg-accent" />
          <ul>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
            <li className="h-14 w-full border-t bg-accent/30"></li>
          </ul>
        </div>
      </div>
    </div>
  );
};

export default AccountsLoadingSkeleton;
