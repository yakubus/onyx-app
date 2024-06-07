const BudgetsLoadingSkeleton = () => {
  return (
    <div className="h-full animate-pulse overflow-auto py-8 scrollbar-none md:pl-14 md:pr-10 md:pt-14">
      <header className="space-y-1 lg:space-y-2">
        <div className="h-14 w-2/3 bg-primary/10" />
        <div className="h-2 w-1/2 bg-primary/10" />
      </header>
      <div className="space-y-4 py-20">
        <div className="overflow-hidden rounded-lg border">
          <div className="h-14 w-full bg-primary/10" />
          <ul>
            <li className="h-20 w-full border-t bg-accent/30"></li>
            <li className="h-20 w-full border-t bg-accent/30"></li>
            <li className="h-20 w-full border-t bg-accent/30"></li>
            <li className="h-20 w-full border-t bg-accent/30"></li>
          </ul>
        </div>
        <div className="flex w-full justify-center">
          <div className="h-10 w-10 rounded-full bg-primary/10"></div>
        </div>
      </div>
    </div>
  );
};

export default BudgetsLoadingSkeleton;
