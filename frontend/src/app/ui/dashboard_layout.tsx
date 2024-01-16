import { ReactNode } from 'react';
import Sidebar from './Sidebar';

interface DashboardLayoutProps {
  children: ReactNode;
}

const DashboardLayout: React.FC<DashboardLayoutProps> = ({ children }) => {
  return (
    <div className="flex bg-white">
      <Sidebar />
      <div className="flex-1 p-4">{children}</div>
    </div>
  );
};

export default DashboardLayout;
