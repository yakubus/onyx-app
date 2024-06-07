import { Card, CardContent } from "@/components/ui/card";

const SingleBudgetLoadingSkeleton = () => {
  return (
    <div className="grid h-full grid-cols-1 gap-x-8 gap-y-4 rounded-md lg:grid-cols-5">
      <div className="flex h-full animate-pulse flex-col space-y-4 overflow-hidden lg:col-span-2">
        <Card>
          <div className="p-6">
            <div className="flex justify-between space-y-1.5">
              <div className="space-y-1.5">
                <p className="h-12 w-36 rounded-lg bg-primary/10" />
                <p className="h-4 w-72 rounded-lg bg-primary/10" />
              </div>
              <div className="w-8 rounded-lg bg-primary/10" />
            </div>
          </div>
          <CardContent>
            <div className="h-20 w-full rounded-lg bg-primary/10" />
          </CardContent>
        </Card>
        <Card className="h-full flex-grow">
          <div className="border-b p-6">
            <div className="space-y-1.5">
              <div className="mx-auto h-16 w-48 rounded-lg bg-primary/10" />
            </div>
          </div>
          <CardContent>
            <ul className="space-y-4 py-6">
              <li className="h-14 w-full bg-primary/10"></li>
              <li className="h-14 w-full bg-primary/10"></li>
              <li className="h-14 w-full bg-primary/10"></li>
              <li className="h-14 w-full bg-primary/10"></li>
              <li className="h-14 w-full bg-primary/10"></li>
            </ul>
          </CardContent>
        </Card>
      </div>
    </div>
  );
};

export default SingleBudgetLoadingSkeleton;
