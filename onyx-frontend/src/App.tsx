import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import { RouterProvider, createRouter } from "@tanstack/react-router";
import { routeTree } from "./routeTree.gen";
import { useAuth } from "./lib/hooks/useAuth";
import { AuthProvider } from "./components/AuthProvider";

declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router;
  }
}

const queryClient = new QueryClient();
const router = createRouter({
  routeTree,
  context: { queryClient, auth: undefined! },
  defaultPreload: "intent",
  defaultPreloadStaleTime: 0,
});

const App = () => {
  return (
    <QueryClientProvider client={queryClient}>
      <AuthProvider>
        <RouterWithAuth />
        <ReactQueryDevtools initialIsOpen={false} />
      </AuthProvider>
    </QueryClientProvider>
  );
};

export default App;

const RouterWithAuth = () => {
  const auth = useAuth();
  const routerContext = { queryClient, auth };
  return <RouterProvider router={router} context={routerContext} />;
};
