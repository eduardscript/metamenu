const Modal: React.FC<{ onClose: () => void; children: React.ReactNode }> = ({ onClose, children }) => {
  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-white p-4 rounded shadow-md w-full sm:w-96 md:w-1/2 lg:w-2/3 xl:w-1/3">
        <div className="mt-4">{children}</div>
      </div>
    </div>
  );
};

export default Modal;
