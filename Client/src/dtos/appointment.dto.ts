export type AppointmentDto = {
  appointmentId: number;
  UserId:number;
  clientName: string;
  scheduledTime: string;
  createdAt: string;
  finalPrice: number;
  dogSize: string;
  typeId: number;
};

export type CreateAppointmentDto = {
  typeId: number;
  scheduledTime: string;
};
