import React from "react";

const ErrorMessage = ({ error }) => {
  if (!error) return null;
  return <div>{error}</div>;
};

export default ErrorMessage;
