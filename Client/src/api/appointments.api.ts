import { http } from "./http";
import type { AppointmentDto, CreateAppointmentDto } 
  from "../dtos/appointment.dto";
  import  type  { AppointmentTypeDto } from '../dtos/appointment-type.dto';


export const appointmentsApi = {
  getAll() {
    return http.get<AppointmentDto[]>("/appointments");
  },

  getTypes() {
    return http.get<AppointmentTypeDto[]>(
      "/appointments/types"
    );
  },

  create(data: CreateAppointmentDto) {
    return http.post<AppointmentDto>(
      "/appointments",
      data
    );
  },
    update(id: number, data: CreateAppointmentDto) {
    return http.put<boolean>(`/appointments/${id}`, data);
  },

  delete(id: number) {
    return http.delete<boolean>(`/appointments/${id}`);
  },
};
